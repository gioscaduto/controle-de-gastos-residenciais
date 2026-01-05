import type { Categoria, CategoriaFinalidade } from '../../domain/entities/categoria'
import { apiClient } from '../../infrastructure/http/apiClient'

export type PaginationParams = {
  pagina?: number
  qtdItensPorPagina?: number
}

const basePath = '/api/v1/categorias'

export async function listCategorias(params?: PaginationParams) {
  const response = await apiClient.get<Categoria[]>(basePath, {
    params: {
      Pagina: params?.pagina,
      QtdItensPorPagina: params?.qtdItensPorPagina,
    },
  })

  return response.data
}

export async function createCategoria(payload: {
  descricao: string
  finalidade: CategoriaFinalidade
}) {
  const response = await apiClient.post<Categoria>(basePath, payload)
  return response.data
}

export async function updateCategoria(
  id: string,
  payload: { id?: string; descricao: string; finalidade: CategoriaFinalidade },
) {
  const response = await apiClient.put<Categoria>(`${basePath}/${id}`, payload)
  return response.data
}

export async function deleteCategoria(id: string) {
  await apiClient.delete(`${basePath}/${id}`)
}
