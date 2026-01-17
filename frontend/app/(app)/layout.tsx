import type { Metadata } from "next"

export const metadata: Metadata = {
  title: "TodoApp - Quản lý công việc thông minh",
  description: "Biến ý tưởng thành hành động, task thành thành tựu. Tăng năng suất làm việc với công cụ quản lý todo đơn giản nhưng mạnh mẽ.",
  keywords: ["todo", "task management", "productivity", "quản lý công việc", "năng suất"],
  authors: [{ name: "TodoApp Team" }],
  openGraph: {
    title: "TodoApp - Quản lý công việc thông minh",
    description: "Biến ý tưởng thành hành động, task thành thành tựu.",
    type: "website",
  },
}

export default function AppLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return children
}
