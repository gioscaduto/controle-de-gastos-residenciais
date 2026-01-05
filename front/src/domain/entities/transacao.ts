export const TransacaoTipo = {
  Despesa: 1,
  Receita: 2,
} as const

export type TransacaoTipo = (typeof TransacaoTipo)[keyof typeof TransacaoTipo]

export type Transacao = {
  id?: string
  descricao: string
  valor: number
  categoriaId: string
  pessoaId: string
  tipo: TransacaoTipo
}

export const transacaoTipoLabels: Record<TransacaoTipo, string> = {
  [TransacaoTipo.Despesa]: 'Despesa',
  [TransacaoTipo.Receita]: 'Receita',
}

export const transacaoTipoOptions = Object.entries(transacaoTipoLabels).map(([value, label]) => ({
  label,
  value: Number(value) as TransacaoTipo,
}))

export type TransacaoPessoaResumo = {
  pessoa: string | null
  totalReceita: number
  totalDespesa: number
  saldo: number
}

export type TransacaoCategoriaResumo = {
  categoria: string | null
  totalReceita: number
  totalDespesa: number
  saldo: number
}
