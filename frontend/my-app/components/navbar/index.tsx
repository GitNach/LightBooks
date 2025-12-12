import Link from "next/link";
import { LoginButton } from "@/components/button/LoginButton";

import { isAuthenticated } from "@/lib/auth";
import { LogoutButton } from "../button/LogoutButton";
const navItems = [
  {
    label: "Home",
    href: "/",
  },
  {
    label: "Books",
    href: "/books",
  },
  {
    label: "Contact",
    href: "/contact",
  },
];

export default async function Navbar() {
  const authenticated = await isAuthenticated();
  return (
    <div className="w-full h-16 bg-zinc-500">
      <nav className="w-full flex items-center  px-6 py-4 bg-gray-900 text-whit ">
        <div className="flex space-x-10">
          {navItems.map((item) => (
            <Link
              href={item.href}
              key={item.href}
              className="hover:text-zinc-400"
            >
              {item.label}
            </Link>
          ))}
        </div>
        <div className="ml-auto flex space-x-2">
          {authenticated ? (
            <LogoutButton />
          ) : (
            <>
              <LoginButton variant="login" />
              <LoginButton variant="register" />
            </>
          )}
        </div>
      </nav>
    </div>
  );
}
