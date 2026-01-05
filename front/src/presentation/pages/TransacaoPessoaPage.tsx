import { useMemo } from 'react'
import { App, Table, Tag } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import { useQuery } from '@tanstack/react-query'
import { PageHeader } from '../components/PageHeader'
import type { TransacaoPessoaResumo } from '../../domain/entities/transacao'
import { listTransacoesPorPessoa } from '../../application/services/transacaoService'
import { formatarMoeda } from '../shared/moeda'

export function TransacaoPessoaPage() {
  const { message } = App.useApp()

  const transacoesPessoaQuery = useQuery<TransacaoPessoaResumo[]>({
    queryKey: ['transacoes', 'pessoa'],
    queryFn: listTransacoesPorPessoa,
    placeholderData: (previousData) => previousData ?? [],
    onError: () => message.error('Não foi possível carregar as transações por pessoa'),
    refetchOnMount: 'always',
    refetchOnWindowFocus: true,
    refetchOnReconnect: true,
    staleTime: 0,
  })

  const totais = useMemo(() => {
    const data = transacoesPessoaQuery.data ?? []
    return data.reduce(
      (acc, item) => {
        acc.totalReceita += item.totalReceita ?? 0
        acc.totalDespesa += item.totalDespesa ?? 0
        acc.saldo += item.saldo ?? 0
        return acc
      },
      { totalReceita: 0, totalDespesa: 0, saldo: 0 },
    )
  }, [transacoesPessoaQuery.data])

  const columns: ColumnsType<TransacaoPessoaResumo> = useMemo(
    () => [
      {
        title: 'Pessoa',
        dataIndex: 'pessoa',
        key: 'pessoa',
        render: (pessoa?: string | null) => pessoa ?? '-',
      },
      {
        title: 'Receitas',
        dataIndex: 'totalReceita',
        key: 'totalReceita',
        render: (valor: number) => formatarMoeda(valor),
      },
      {
        title: 'Despesas',
        dataIndex: 'totalDespesa',
        key: 'totalDespesa',
        render: (valor: number) => formatarMoeda(valor),
      },
      {
        title: 'Saldo',
        dataIndex: 'saldo',
        key: 'saldo',
        render: (saldo: number) => (
          <Tag color={saldo < 0 ? 'red' : 'green'}>{formatarMoeda(saldo)}</Tag>
        ),
      },
    ],
    [],
  )

  return (
    <>
      <PageHeader title="Transações por pessoa" subtitle="Totais consolidados por pessoa" />

      <Table
        rowKey={(item) => item.pessoa}
        style={{ marginTop: 16 }}
        loading={transacoesPessoaQuery.isLoading}
        dataSource={transacoesPessoaQuery.data ?? []}
        columns={columns}
        pagination={false}
        summary={() => (
          <Table.Summary fixed>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0}></Table.Summary.Cell>
              <Table.Summary.Cell index={1}>
                <strong>{formatarMoeda(totais.totalReceita)}</strong>
              </Table.Summary.Cell>
              <Table.Summary.Cell index={2}>
                <strong>{formatarMoeda(totais.totalDespesa)}</strong>
              </Table.Summary.Cell>
              <Table.Summary.Cell index={3}>
                <strong>{formatarMoeda(totais.saldo)}</strong>
              </Table.Summary.Cell>
            </Table.Summary.Row>
          </Table.Summary>
        )}
      />
    </>
  )
}
