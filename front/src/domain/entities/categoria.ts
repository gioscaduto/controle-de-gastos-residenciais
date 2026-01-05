export const CategoriaFinalidade = {
  Despesa: 1,
  Receita: 2,
  Ambas: 3,
} as const

export type CategoriaFinalidade = (typeof CategoriaFinalidade)[keyof typeof CategoriaFinalidade]

export type Categoria = {
  id?: string
  descricao: string
  finalidade: CategoriaFinalidade
}

export const categoriaFinalidadeLabels: Record<CategoriaFinalidade, string> = {
  [CategoriaFinalidade.Despesa]: 'Despesa',
  [CategoriaFinalidade.Receita]: 'Receita',
  [CategoriaFinalidade.Ambas]: 'Ambas',
}

export const categoriaFinalidadeOptions = Object.entries(categoriaFinalidadeLabels).map(
  ([value, label]) => ({
    label,
    value: Number(value) as CategoriaFinalidade,
  }),
)
