"use client";

import Link from "next/link";

type Variant = "login" | "register";

interface AuthButtonProps {
	variant?: Variant;
	className?: string;
	fullWidth?: boolean;
	
}

const LABEL: Record<Variant, string> = {
	login: "Iniciar sesión",
	register: "Registrarse",
};

const HREF: Record<Variant, string> = {
	login: "/login",
	register: "/register",
};

export function LoginButton({ variant = "login", className, fullWidth }: AuthButtonProps) {
	const baseClasses =
		"inline-flex items-center justify-center rounded bg-blue-500 px-4 py-2 text-sm font-semibold text-white shadow hover:bg-blue-600 transition";
	const widthClasses = fullWidth ? " w-full" : "";
	const extraClasses = className ? ` ${className}` : "";

	return (
		<Link
			href={HREF[variant]}
			className={`${baseClasses}${widthClasses}${extraClasses}`}
		>
			{LABEL[variant]}
		</Link>
	);
}
