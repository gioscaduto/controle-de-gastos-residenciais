import { useMemo, useState } from 'react'
import { App, Button, Flex, Form, Input, Modal, Popconfirm, Select, Table, Tag } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import type { Categoria } from '../../domain/entities/categoria'
import {
  CategoriaFinalidade,
  categoriaFinalidadeLabels,
  categoriaFinalidadeOptions,
} from '../../domain/entities/categoria'
import {
  createCategoria,
  deleteCategoria,
  listCategorias,
  updateCategoria,
} from '../../application/services/categoriaService'
import { PageHeader } from '../components/PageHeader'
import axios from 'axios'

const defaultPagination = { current: 1, pageSize: 25 }

type CategoriaFormValues = {
  id?: string
  descricao: string
  finalidade: CategoriaFinalidade
}


export function CategoriaPage() {
  const { message } = App.useApp()
  const queryClient = useQueryClient()
  const [form] = Form.useForm<CategoriaFormValues>()
  const [modalOpen, setModalOpen] = useState(false)
  const [editing, setEditing] = useState<Categoria | null>(null)
  const [deletingId, setDeletingId] = useState<string | null>(null)

  const categoriasQuery = useQuery<Categoria[]>({
    queryKey: ['categorias', defaultPagination],
    queryFn: () =>
      listCategorias({
        pagina: defaultPagination.current,
        qtdItensPorPagina: defaultPagination.pageSize,
      }),
    placeholderData: (previousData) => previousData ?? [],
  })

  const createOrUpdate = useMutation({
    mutationFn: async (values: CategoriaFormValues) => {
      if (editing?.id) {
        return updateCategoria(editing.id, values)
      }

      return createCategoria(values)
    },
    onSuccess: () => {
      message.success('Categoria salva com sucesso')
      queryClient.invalidateQueries({ queryKey: ['categorias'] })
      setModalOpen(false)
      setEditing(null)
      form.resetFields()
    },
    
    onError: (error) => { 
      const errorMessage =
          axios.isAxiosError<{ message?: string; errors?: string[] }>(error)
            ? error.response?.data?.message ?? error.response?.data?.errors?.join(', ')
            : null

      message.error(errorMessage || 'Não foi possível salvar a categoria')
    }
  })

  const removeMutation = useMutation({
    mutationFn: async (id: string) => deleteCategoria(id),
    onSuccess: () => {
      message.success('Categoria removida')
      queryClient.invalidateQueries({ queryKey: ['categorias'] })
    },
    onError: () => message.error('Não foi possível remover a categoria'),
    onSettled: () => setDeletingId(null),
  })

  const columns: ColumnsType<Categoria> = useMemo(
    () => [
      {
        title: 'Descrição',
        dataIndex: 'descricao',
        key: 'descricao',
      },
      {
        title: 'Finalidade',
        dataIndex: 'finalidade',
        key: 'finalidade',
        render: (finalidade: CategoriaFinalidade) => {
          const label = categoriaFinalidadeLabels[finalidade]
          return <Tag color="blue">{label ?? finalidade}</Tag>
        },
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

  function handleEdit(item: Categoria) {
    setEditing(item)
    form.setFieldsValue({
      id: item.id,
      descricao: item.descricao,
      finalidade: item.finalidade,
    })
    setModalOpen(true)
  }

  function handleDelete(item: Categoria) {
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
      <PageHeader title="Categorias" subtitle="Cadastre e organize as categorias das transações">
        <Button type="primary" onClick={handleNew}>
          Nova categoria
        </Button>
      </PageHeader>

      <Table
        style={{ marginTop: 16 }}
        rowKey={(item) => item.id}
        loading={categoriasQuery.isLoading}
        dataSource={categoriasQuery.data ?? []}
        columns={columns}
      />

      <Modal
        title={editing ? 'Editar categoria' : 'Nova categoria'}
        open={modalOpen}
        okText={editing ? 'Salvar alterações' : 'Criar categoria'}
        onCancel={() => {
          setModalOpen(false)
          setEditing(null)
        }}
        onOk={handleSubmit}
        confirmLoading={createOrUpdate.isPending}
        destroyOnHidden
      >
        <Form layout="vertical" form={form} initialValues={{ finalidade: CategoriaFinalidade.Despesa }}>
          {editing && editing.id && (
            <Form.Item label="ID" name="id">
              <Input value={editing.id} disabled />
            </Form.Item>
          )}
          
          <Form.Item
            label="Descrição"
            name="descricao"
            rules={[{ required: true, message: 'Informe a descrição' }, { min: 2, max: 255 }]}
          >
            <Input placeholder="Ex.: Mercado" />
          </Form.Item>
          <Form.Item
            label="Finalidade"
            name="finalidade"
            rules={[{ required: true, message: 'Selecione a finalidade' }]}
          >
            <Select options={categoriaFinalidadeOptions} placeholder="Selecione" />
          </Form.Item>
        </Form>
      </Modal>
    </>
  )
}
