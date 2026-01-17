import type { Metadata } from "next"
import Link from "next/link"
import { CheckSquare } from "lucide-react"

export const metadata: Metadata = {
  title: "Xác thực - TodoApp",
  description: "Đăng nhập hoặc đăng ký tài khoản TodoApp",
}

export default function AuthLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <div className="min-h-screen grid lg:grid-cols-2">
      {/* Left Panel - Branding */}
      <div className="relative hidden lg:flex flex-col bg-gradient-to-br from-violet-600 via-fuchsia-600 to-pink-600 p-10 text-white">
        {/* Pattern Background */}
        <div className="absolute inset-0 bg-[url('data:image/svg+xml;base64,PHN2ZyB3aWR0aD0iNjAiIGhlaWdodD0iNjAiIHZpZXdCb3g9IjAgMCA2MCA2MCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48ZyBmaWxsPSJub25lIiBmaWxsLXJ1bGU9ImV2ZW5vZGQiPjxwYXRoIGQ9Ik0zNiAxOGMtOS45NDEgMC0xOCA4LjA1OS0xOCAxOHM4LjA1OSAxOCAxOCAxOCAxOC04LjA1OSAxOC0xOC04LjA1OS0xOC0xOC0xOHptMCAzMmMtNy43MzIgMC0xNC02LjI2OC0xNC0xNHM2LjI2OC0xNCAxNC0xNCAxNCA2LjI2OCAxNCAxNC02LjI2OCAxNC0xNCAxNHoiIGZpbGw9IiNmZmYiIGZpbGwtb3BhY2l0eT0iLjA1Ii8+PC9nPjwvc3ZnPg==')] opacity-30" />

        {/* Logo */}
        <Link href="/" className="relative z-10 flex items-center gap-2">
          <div className="flex items-center justify-center w-10 h-10 rounded-xl bg-white/20 backdrop-blur-sm">
            <CheckSquare className="w-6 h-6 text-white" />
          </div>
          <span className="text-2xl font-bold">TodoApp</span>
        </Link>

        {/* Content */}
        <div className="relative z-10 flex flex-col justify-center flex-1">
          <blockquote className="space-y-4">
            <p className="text-2xl font-medium leading-relaxed">
              &ldquo;TodoApp đã giúp team tôi tăng năng suất 40%. 
              Giao diện đẹp, dễ sử dụng và tính năng cộng tác tuyệt vời!&rdquo;
            </p>
            <footer className="flex items-center gap-3">
              <div className="w-12 h-12 rounded-full bg-white/20 flex items-center justify-center text-lg font-semibold">
                MA
              </div>
              <div>
                <div className="font-semibold">Minh Anh</div>
                <div className="text-white/70 text-sm">Product Manager @ TechCorp</div>
              </div>
            </footer>
          </blockquote>
        </div>

        {/* Stats */}
        <div className="relative z-10 grid grid-cols-3 gap-6 pt-8 border-t border-white/20">
          <div>
            <div className="text-3xl font-bold">10K+</div>
            <div className="text-white/70 text-sm">Người dùng</div>
          </div>
          <div>
            <div className="text-3xl font-bold">50K+</div>
            <div className="text-white/70 text-sm">Tasks hoàn thành</div>
          </div>
          <div>
            <div className="text-3xl font-bold">99%</div>
            <div className="text-white/70 text-sm">Hài lòng</div>
          </div>
        </div>
      </div>

      {/* Right Panel - Form */}
      <div className="flex flex-col">
        {/* Mobile Logo */}
        <div className="lg:hidden p-6">
          <Link href="/" className="flex items-center gap-2">
            <div className="flex items-center justify-center w-9 h-9 rounded-lg bg-gradient-to-br from-violet-600 to-fuchsia-600 shadow-lg shadow-violet-500/25">
              <CheckSquare className="w-5 h-5 text-white" />
            </div>
            <span className="text-xl font-bold tracking-tight bg-gradient-to-r from-violet-600 to-fuchsia-600 bg-clip-text text-transparent">
              TodoApp
            </span>
          </Link>
        </div>

        {/* Form Container */}
        <div className="flex-1 flex items-center justify-center p-6 lg:p-10">
          <div className="w-full max-w-md">
            {children}
          </div>
        </div>
      </div>
    </div>
  )
}
