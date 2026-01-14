"use client"

import * as React from "react"
import { useTheme } from "next-themes"
import { Button } from "@/registry/new-york-v4/ui/button"

export default function Page() {
  const { theme, setTheme } = useTheme()
  const [mounted, setMounted] = React.useState(false)

  React.useEffect(() => {
    setMounted(true)
  }, [])

  if (!mounted) return null

  return (
    <Button
      onClick={() =>
        setTheme(theme === "dark" ? "light" : "dark")
      }
    >
      Toggle theme
    </Button>
  )
}
