import type { PropsWithChildren } from 'react'
import { Flex, Space, Typography } from 'antd'

type PageHeaderProps = PropsWithChildren<{
  title: string
  subtitle?: string
}>

export function PageHeader({ title, subtitle, children }: PageHeaderProps) {
  return (
    <Flex justify="space-between" align="center" className="page-header">
      <div>
        <Typography.Title level={3} style={{ margin: 0 }}>
          {title}
        </Typography.Title>
        {subtitle ? (
          <Typography.Text type="secondary" className="page-subtitle">
            {subtitle}
          </Typography.Text>
        ) : null}
      </div>
      {children ? <Space>{children}</Space> : null}
    </Flex>
  )
}
