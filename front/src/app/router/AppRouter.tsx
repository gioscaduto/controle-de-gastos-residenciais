import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { MainLayout } from '../../presentation/layouts/MainLayout'
import { CategoriaPage } from '../../presentation/pages/CategoriasPage'
import { PessoaPage } from '../../presentation/pages/PessoaPage'
import { TransacaoPage } from '../../presentation/pages/TransacaoPage'
import { TransacaoPessoaPage } from '../../presentation/pages/TransacaoPessoaPage'
import { TransacaoCategoriaPage } from '../../presentation/pages/TransacaoCategoriaPage'
import { Result } from 'antd'

export function AppRouter() {
  return (
    <BrowserRouter>
      <MainLayout>
        <Routes>
          <Route path="/" element={<Navigate to="/categorias" replace />} />
          <Route path="/categorias" element={<CategoriaPage />} />
          <Route path="/pessoas" element={<PessoaPage />} />
          <Route path="/transacoes" element={<TransacaoPage />} />
          <Route path="/transacoes/pessoa" element={<TransacaoPessoaPage />} />
          <Route path="/transacoes/categoria" element={<TransacaoCategoriaPage />} />
          <Route
            path="*"
            element={<Result status="404" title="Página não encontrada" subTitle="Verifique o endereço e tente novamente." />}
          />
        </Routes>
      </MainLayout>
    </BrowserRouter>
  )
}
