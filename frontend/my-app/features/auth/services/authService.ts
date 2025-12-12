import { env } from '../../../config/env';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface TokenResponse {
  token: string;
  expiresIn: string;
}

export const loginUser = async (data: LoginRequest): Promise<TokenResponse> => {
  const response = await fetch(`${env.NEXT_PUBLIC_BACKEND_API_URL}/api/auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Error al iniciar sesión');
  }

  return response.json();
};

export const registerUser = async (data: RegisterRequest): Promise<void> => {
  const response = await fetch(`${env.NEXT_PUBLIC_BACKEND_API_URL}/api/auth/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });

  if (!response.ok) {
    const error = await response.json();
    throw new Error(error.message || 'Error al registrarse');
  }
};