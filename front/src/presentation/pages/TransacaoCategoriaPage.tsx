import { useMemo } from 'react'
import { Table, Tag } from 'antd'
import type { ColumnsType } from 'antd/es/table'
import { useQuery } from '@tanstack/react-query'
import { PageHeader } from '../components/PageHeader'
import type { TransacaoCategoriaResumo } from '../../domain/entities/transacao'
import { listTransacoesPorCategoria } from '../../application/services/transacaoService'
import { formatarMoeda } from '../shared/moeda'

export function TransacaoCategoriaPage() {
  const transacoesCategoriaQuery = useQuery<TransacaoCategoriaResumo[]>({
    queryKey: ['transacoes', 'categoria'],
    queryFn: listTransacoesPorCategoria,
    placeholderData: (previousData) => previousData ?? [],
    refetchOnMount: 'always',
    refetchOnWindowFocus: true,
    refetchOnReconnect: true,
    staleTime: 0,
  })

  const totais = useMemo(() => {
    const data = transacoesCategoriaQuery.data ?? []
    return data.reduce(
      (acc, item) => {
        acc.totalReceita += item.totalReceita ?? 0
        acc.totalDespesa += item.totalDespesa ?? 0
        acc.saldo += item.saldo ?? 0
        return acc
      },
      { totalReceita: 0, totalDespesa: 0, saldo: 0 },
    )
  }, [transacoesCategoriaQuery.data])

  const columns: ColumnsType<TransacaoCategoriaResumo> = useMemo(
    () => [
      {
        title: 'Categoria',
        dataIndex: 'categoria',
        key: 'categoria',
        render: (categoria?: string | null) => categoria ?? '-',
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
      <PageHeader title="Transações por categoria" subtitle="Totais consolidados por categoria" />

      <Table
        rowKey={(item) => item.categoria!}
        style={{ marginTop: 16 }}
        loading={transacoesCategoriaQuery.isLoading}
        dataSource={transacoesCategoriaQuery.data ?? []}
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
