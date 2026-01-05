import type { Pessoa } from '../../domain/entities/pessoa'
import { apiClient } from '../../infrastructure/http/apiClient'
import type { PaginationParams } from './categoriaService'

const basePath = '/api/v1/pessoas'

export async function listPessoas(params?: PaginationParams) {
  const response = await apiClient.get<Pessoa[]>(basePath, {
    params: {
      Pagina: params?.pagina,
      QtdItensPorPagina: params?.qtdItensPorPagina,
    },
  })

  return response.data
}

export async function createPessoa(payload: { nome: string; idade?: number | null }) {
  const response = await apiClient.post<Pessoa>(basePath, payload)
  return response.data
}

export async function updatePessoa(id: string, payload: { nome: string; idade?: number | null }) {
  const response = await apiClient.put<Pessoa>(`${basePath}/${id}`, payload)
  return response.data
}

export async function deletePessoa(id: string) {
  await apiClient.delete(`${basePath}/${id}`)
}
