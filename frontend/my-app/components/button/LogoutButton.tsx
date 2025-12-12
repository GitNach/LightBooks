"use client";
import { clearAuthToken } from "@/lib/auth";
import { redirect } from "next/navigation";

interface Props {
  className?: string;
  fullWidth?: boolean;
}

export function LogoutButton({ className, fullWidth }: Props) {
  const baseClasses =
    "inline-flex items-center justify-center rounded bg-blue-500 px-4 py-2 text-sm font-semibold text-white shadow hover:bg-blue-600 transition cursor-pointer";
  const widthClasses = fullWidth ? " w-full" : "";
  const extraClasses = className ? ` ${className}` : "";
  const handleLogout = () => {
    clearAuthToken();
    redirect("/");
  };

  return (
    <button
      onClick={handleLogout}
      className={`${baseClasses}${widthClasses}${extraClasses}`}
    >
      Cerrar sesión
    </button>
  );
}
