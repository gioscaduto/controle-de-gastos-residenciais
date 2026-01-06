import { useMemo, useState } from 'react'
import { App, Button, Flex, Form, Input, InputNumber, Modal, Popconfirm, Table } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import type { Pessoa } from '../../domain/entities/pessoa'
import { createPessoa, deletePessoa, listPessoas, updatePessoa } from '../../application/services/pessoaService'
import { PageHeader } from '../components/PageHeader'
import axios from 'axios'

const defaultPagination = { current: 1, pageSize: 25 }

type PessoaFormValues = {
  id?: string
  nome: string
  idade?: number | null
}

export function PessoaPage() {
  const { message } = App.useApp()
  const queryClient = useQueryClient()
  const [form] = Form.useForm<PessoaFormValues>()
  const [modalOpen, setModalOpen] = useState(false)
  const [editing, setEditing] = useState<Pessoa | null>(null)
  const [deletingId, setDeletingId] = useState<string | null>(null)

  const pessoasQuery = useQuery<Pessoa[]>({
    queryKey: ['pessoas', defaultPagination],
    queryFn: () =>
      listPessoas({
        pagina: defaultPagination.current,
        qtdItensPorPagina: defaultPagination.pageSize,
      }),
    placeholderData: (previousData) => previousData ?? [],
    refetchOnMount: 'always',
    refetchOnWindowFocus: true,
    refetchOnReconnect: true,
    staleTime: 0,
  })

  const createOrUpdate = useMutation({
    mutationFn: async (values: PessoaFormValues) => {
      const payload = { ...values, idade: values.idade ?? null }
      if (editing?.id) {
        return updatePessoa(editing.id, payload)
      }

      return createPessoa(payload)
    },
    onSuccess: () => {
      message.success('Pessoa salva com sucesso')
      queryClient.invalidateQueries({ queryKey: ['pessoas'] })
      setModalOpen(false)
      setEditing(null)
      form.resetFields()
    },
    onError: (error) => 
      {
       const errorMessage =
          axios.isAxiosError<{ message?: string; errors?: string[] }>(error)
            ? error.response?.data?.message ?? error.response?.data?.errors?.join(', ')
            : null

        message.error(errorMessage || 'Não foi possível salvar a pessoa')
      }
  })

  const removeMutation = useMutation({
    mutationFn: async (id: string) => deletePessoa(id),
    onSuccess: () => {
      message.success('Pessoa removida')
      queryClient.invalidateQueries({ queryKey: ['pessoas'] })
    },
    onError: () => message.error('Não foi possível remover a pessoa'),
    onSettled: () => setDeletingId(null),
  })

  const columns: ColumnsType<Pessoa> = useMemo(
    () => [
      {
        title: 'Nome',
        dataIndex: 'nome',
        key: 'nome',
      },
      {
        title: 'Idade',
        dataIndex: 'idade',
        key: 'idade'
      },
      {
        title: 'Ações',
        key: 'acoes',
        render: (_, item) => (
          <Flex gap="small">
            <Button type="primary" onClick={() => handleEdit(item)}>
              Editar
            </Button>
            <Popconfirm
              title="Confirmar exclusão"
              description="Essa ação não poderá ser desfeita."
              okText="Excluir"
              okButtonProps={{ danger: true, loading: removeMutation.isPending && deletingId === item.id }}
              cancelText="Cancelar"
              onConfirm={() => handleDelete(item)}
            >
              <Button type="primary" danger loading={removeMutation.isPending && deletingId === item.id}>
                Excluir
              </Button>
            </Popconfirm>
          </Flex>
        ),
      },
    ],
    [deletingId, removeMutation.isPending],
  )

  function handleNew() {
    setEditing(null)
    form.resetFields()
    setModalOpen(true)
  }

  function handleEdit(item: Pessoa) {
    setEditing(item)
    form.setFieldsValue({ id: item.id, nome: item.nome, idade: item.idade })
    setModalOpen(true)
  }

  function handleDelete(item: Pessoa) {
    if (!item.id) return
    setDeletingId(item.id)
    removeMutation.mutate(item.id)
  }

  async function handleSubmit() {
    try {
      const values = await form.validateFields()
      await createOrUpdate.mutateAsync(values)
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.log(error)
        return
      }
      if (error instanceof Error) {
        message.error(error.message)
      }
    }
  }

  return (
    <>
      <PageHeader title="Pessoas" subtitle="Cadastre os responsáveis pelas transações">
        <Button type="primary" onClick={handleNew}>
          Nova pessoa
        </Button>
      </PageHeader>

      <Table
        style={{ marginTop: 16 }}
        rowKey={(item) => item.id!}
        loading={pessoasQuery.isLoading}
        dataSource={pessoasQuery.data ?? []}
        columns={columns}
      />

      <Modal
        title={editing ? 'Editar pessoa' : 'Nova pessoa'}
        open={modalOpen}
        okText={editing ? 'Salvar alterações' : 'Criar pessoa'}
        onCancel={() => {
          setModalOpen(false)
          setEditing(null)
        }}
        onOk={handleSubmit}
        confirmLoading={createOrUpdate.isPending}
        destroyOnHidden
      >
        <Form layout="vertical" form={form} initialValues={{ idade: null }}>
          {editing && editing.id && (
            <Form.Item label="ID" name="id">
              <Input value={editing.id} disabled />
            </Form.Item>
          )}
          <Form.Item label="Nome" name="nome" rules={[{ required: true, message: 'Informe o nome' }, { min: 2, max: 255 }]}>
            <Input placeholder="Ex.: Ana Paula" />
          </Form.Item>
          <Form.Item
            label="Idade"
            name="idade"
            rules={[{ type: 'number', required: true, message: 'Informe a idade', min: 0, max: 999 }]}
          >
            <InputNumber style={{ width: '100%' }} placeholder="Informe a idade" min={0} max={999} />
          </Form.Item>
        </Form>
      </Modal>
    </>
  )
}
