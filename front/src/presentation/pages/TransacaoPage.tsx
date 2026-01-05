import { useMemo, useState } from 'react'
import { App, Button, Form, Input, InputNumber, Modal, Select, Table, Tag } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import axios from 'axios'
import { PageHeader } from '../components/PageHeader'
import type { Transacao } from '../../domain/entities/transacao'
import { TransacaoTipo, transacaoTipoLabels, transacaoTipoOptions } from '../../domain/entities/transacao'
import type { Categoria } from '../../domain/entities/categoria'
import type { Pessoa } from '../../domain/entities/pessoa'
import { listCategorias } from '../../application/services/categoriaService'
import { listPessoas } from '../../application/services/pessoaService'
import { createTransacao, listTransacao } from '../../application/services/transacaoService'
import TextArea from 'antd/lib/input/TextArea'
import { formatarMoeda } from '../shared/moeda'

const defaultPagination = { current: 1, pageSize: 25 }

type TransacaoFormValues = {
  descricao: string
  valor: number 
  categoriaId: string
  pessoaId: string
  tipo: TransacaoTipo
}

export function TransacaoPage() {
  const { message } = App.useApp()
  const queryClient = useQueryClient()
  const [form] = Form.useForm<TransacaoFormValues>()
  const [modalOpen, setModalOpen] = useState(false)
 
  const transacaoQuery = useQuery<Transacao[]>({
    queryKey: ['transacao', defaultPagination],
    queryFn: () =>
      listTransacao({
        pagina: defaultPagination.current,
        qtdItensPorPagina: defaultPagination.pageSize,
      }),
    placeholderData: (previousData) => previousData ?? [],
    refetchOnMount: 'always',
    refetchOnWindowFocus: true,
    refetchOnReconnect: true,
    staleTime: 0,
  })

  const categoriasQuery = useQuery<Categoria[]>({
    queryKey: ['categorias', 'select'],
    queryFn: () => listCategorias({ pagina: 1, qtdItensPorPagina: 1000 }),
    placeholderData: (previousData) => previousData ?? [],
    refetchOnMount: 'always',
    refetchOnWindowFocus: true,
    refetchOnReconnect: true,
    staleTime: 0,
  })

  const pessoasQuery = useQuery<Pessoa[]>({
    queryKey: ['pessoas', 'select'],
    queryFn: () => listPessoas({ pagina: 1, qtdItensPorPagina: 1000 }),
    placeholderData: (previousData) => previousData ?? [],
    refetchOnMount: 'always',
    refetchOnWindowFocus: true,
    refetchOnReconnect: true,
    staleTime: 0,
  })

  const categoriaLookup = useMemo(
    () => new Map((categoriasQuery.data ?? []).map((categoria) => [categoria.id, categoria.descricao])),
    [categoriasQuery.data],
  )

  const pessoaLookup = useMemo(
    () => new Map((pessoasQuery.data ?? []).map((pessoa) => [pessoa.id, pessoa.nome])),
    [pessoasQuery.data],
  )

  const createTransacaoMutation = useMutation({
    mutationFn: async (values: TransacaoFormValues) => {
      const payload = {
        descricao: values.descricao,
        valor: values.valor ?? null,
        categoriaId: values.categoriaId ?? '',
        pessoaId: values.pessoaId ?? '',
        tipo: values.tipo ?? TransacaoTipo.Despesa,
      }

      return createTransacao(payload)
    },
    onSuccess: () => {
      message.success('Transação salva com sucesso')
      queryClient.invalidateQueries({ queryKey: ['transacao'] })
      setModalOpen(false)
      form.resetFields()
    },
    onError: (error) => {
      const errorMessage =
        axios.isAxiosError<{ message?: string; errors?: string[] }>(error)
          ? error.response?.data?.message ?? error.response?.data?.errors?.join(', ')
          : null

      message.error(errorMessage || 'Não foi possível salvar a transação')
    },
  })

  const columns: ColumnsType<Transacao> = useMemo(
    () => [
      {
        title: 'Pessoa',
        dataIndex: 'pessoaId',
        key: 'pessoaId',
        render: (pessoaId: string) => pessoaLookup.get(pessoaId) ?? pessoaId,
      },
      {
        title: 'Descrição',
        dataIndex: 'descricao',
        key: 'descricao',
      },
      {
        title: 'Valor',
        dataIndex: 'valor',
        key: 'valor',
        render: (valor: number) => formatarMoeda(valor),
      },
      {
        title: 'Tipo',
        dataIndex: 'tipo',
        key: 'tipo',
        render: (tipo: TransacaoTipo) => (
          <Tag color={tipo === TransacaoTipo.Despesa ? 'red' : 'green'}>
            {transacaoTipoLabels[tipo] ?? tipo}
          </Tag>
        ),
      },
      {
        title: 'Categoria',
        dataIndex: 'categoriaId',
        key: 'categoriaId',
        render: (categoriaId: string) => categoriaLookup.get(categoriaId) ?? categoriaId,
      }
    ],
    [categoriaLookup, pessoaLookup],
  )

  async function handleSubmit() {
    try {
      const values = await form.validateFields()
      await createTransacaoMutation.mutateAsync(values)
    } catch (error) {
      if (axios.isAxiosError(error)) {
        return
      }
      if (error instanceof Error) {
        message.error(error.message)
      }
    }
  }

  return (
    <>
      <PageHeader title="Transações" subtitle="Consulte e cadastre novas transações">
        <Button type="primary" onClick={() => setModalOpen(true)}>
          Nova transação
        </Button>
      </PageHeader>

      <Table
        rowKey={(item) => item.id}
        style={{ marginTop: 16 }}
        loading={transacaoQuery.isLoading}
        dataSource={transacaoQuery.data ?? []}
        columns={columns}
      />

      <Modal
        title="Nova transação"
        open={modalOpen}
        okText="Criar transação"
        onCancel={() => setModalOpen(false)}
        onOk={handleSubmit}
        confirmLoading={createTransacaoMutation.isPending}
        destroyOnHidden
      >
        <Form layout="vertical" form={form} initialValues={{ tipo: TransacaoTipo.Despesa }}>
          <Form.Item
            label="Descrição"
            name="descricao"
            rules={[{ required: true, message: 'Informe a descrição' }, { min: 2, max: 1000 }]}
          >
            <TextArea rows={4} placeholder="Ex.: Supermercado do mês" />
          </Form.Item>

          <Form.Item
            label="Valor"
            name="valor"
            rules={[{ required: true, type: 'number', min: 0.01, message: 'Informe um valor válido' }]}
          >
            <InputNumber  style={{ width: '100%' }} placeholder="Ex.: 150,90" min={0.01} max={1000000} precision={2} controls={false} />
          </Form.Item>

          <Form.Item label="Tipo" name="tipo" rules={[{ required: true, message: 'Selecione o tipo' }]}>
            <Select options={transacaoTipoOptions} placeholder="Selecione o tipo" />
          </Form.Item>

          <Form.Item
            label="Categoria"
            name="categoriaId"
            rules={[{ required: true, message: 'Selecione a categoria' }]}
          >
            <Select
              loading={categoriasQuery.isLoading}
              options={(categoriasQuery.data ?? []).map((categoria) => ({
                value: categoria.id,
                label: categoria.descricao,
              }))}
              placeholder="Selecione a categoria"
            />
          </Form.Item>

          <Form.Item label="Pessoa" name="pessoaId" rules={[{ required: true, message: 'Selecione a pessoa' }]}>
            <Select
              loading={pessoasQuery.isLoading}
              options={(pessoasQuery.data ?? []).map((pessoa) => ({
                value: pessoa.id,
                label: pessoa.nome,
              }))}
              placeholder="Selecione a pessoa"
            />
          </Form.Item>
        </Form>
      </Modal>
    </>
  )
}
