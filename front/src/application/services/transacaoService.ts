import type {
  Transacao,
  TransacaoCategoriaResumo,
  TransacaoPessoaResumo,
  TransacaoTipo,
} from '../../domain/entities/transacao'
import { apiClient } from '../../infrastructure/http/apiClient'
import type { PaginationParams } from './categoriaService'

const basePath = '/api/v1/transacoes'

export async function listTransacao(params?: PaginationParams) {
  const response = await apiClient.get<Transacao[]>(basePath, {
    params: {
      Pagina: params?.pagina,
      QtdItensPorPagina: params?.qtdItensPorPagina,
    },
  })

  return response.data
}

export async function createTransacao(payload: {
  descricao: string
  valor?: number | null
  categoriaId: string
  pessoaId: string
  tipo: TransacaoTipo
}) {
  const response = await apiClient.post<Transacao>(basePath, payload)
  return response.data
}

export async function listTransacoesPorPessoa() {
  const response = await apiClient.get<TransacaoPessoaResumo[]>(`${basePath}/pessoa`)
  return response.data
}

export async function listTransacoesPorCategoria() {
  const response = await apiClient.get<TransacaoCategoriaResumo[]>(`${basePath}/categoria`)
  return response.data
}
