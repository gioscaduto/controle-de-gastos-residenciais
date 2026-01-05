import { useMemo } from 'react'
import type { PropsWithChildren } from 'react'
import { Layout, Menu } from 'antd'
import { AppstoreOutlined, DollarOutlined, TeamOutlined } from '@ant-design/icons'
import { useLocation, useNavigate } from 'react-router-dom'

const { Sider, Content } = Layout

export function MainLayout({ children }: PropsWithChildren) {
  const navigate = useNavigate()
  const location = useLocation()

  const menuItems = useMemo(
    () => [
      { key: '/categorias', label: 'Categorias', icon: <AppstoreOutlined /> },
      { key: '/pessoas', label: 'Pessoas', icon: <TeamOutlined /> },
      { key: '/transacoes', label: 'Transações', icon: <DollarOutlined /> },
      { key: '/transacoes/pessoa', label: 'Transações por Pessoa', icon: <DollarOutlined /> },
      { key: '/transacoes/categoria', label: 'Transações por Categoria', icon: <DollarOutlined /> },
    ],
    [],
  )

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider className="app-sider" width={250} breakpoint="lg" collapsedWidth={64}>
        <div className="brand sider-brand">Controle de Gastos</div>
        <Menu
          className="app-menu"
          theme="dark"
          mode="inline"
          selectedKeys={[location.pathname]}
          items={menuItems}
          onClick={(item) => navigate(item.key)}
        />
      </Sider>
      <Layout>
        <Content className="app-content">
          <div className="content-card">{children}</div>
        </Content>
      </Layout>
    </Layout>
  )
}
