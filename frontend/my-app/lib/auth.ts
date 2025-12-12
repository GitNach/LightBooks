"use server";
import { jwtDecode } from "jwt-decode";
import { cookies } from "next/headers";

const TOKEN_NAME = "authToken";
const TOKEN_EXPIRY = "authTokenExpiry";

interface JWTPayload {
  sub: string;
  name: string;
  email: string;
  iat: number;
  exp: number;
}

export async function setAuthToken(token: string, expiresIn: string) {
  const cookieStore = await cookies();
  const expiryDate = new Date(expiresIn);

  cookieStore.set(TOKEN_NAME, token, {
    httpOnly: true,
    secure: process.env.NODE_ENV === "production",
    sameSite: "strict",
    expires: expiryDate,
  });

  cookieStore.set(TOKEN_EXPIRY, expiryDate.toISOString(), {
    expires: expiryDate,
  });
}

export async function getAuthToken(): Promise<string | null> {
  const cookieStore = await cookies();
  return cookieStore.get(TOKEN_NAME)?.value || null;
}

export async function clearAuthToken() {
  const cookieStore = await cookies();
  cookieStore.delete(TOKEN_NAME);
  cookieStore.delete(TOKEN_EXPIRY);
}

export async function decodeToken(token: string): Promise<JWTPayload | null> {
  try {
    return jwtDecode<JWTPayload>(token);
  } catch {
    return null;
  }
}

export async function isAuthenticated(): Promise<boolean> {
  const token = await getAuthToken();
  if (!token) return false;

  const decoded = await decodeToken(token);
  if (!decoded) return false;

  // Verificar si el token no ha expirado
  const now = Math.floor(Date.now() / 1000);
  return decoded.exp > now;
}

export async function getCurrentUser(): Promise<{
  id: string;
  email: string;
  name: string;
} | null> {
  const token = await getAuthToken();
  if (!token) return null;

  const decoded = await decodeToken(token);
  if (!decoded) return null;

  return {
    id: decoded.sub,
    email: decoded.email,
    name: decoded.name,
  };
}
