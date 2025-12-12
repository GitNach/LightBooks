import { NextRequest, NextResponse } from 'next/server';
import { jwtDecode } from 'jwt-decode';

const TOKEN_NAME = 'authToken';

interface JWTPayload {
  exp: number;
}

export function middleware(request: NextRequest) {
  const token = request.cookies.get(TOKEN_NAME)?.value;
  const { pathname } = request.nextUrl;

  // Proteger /books/new
  if (pathname.startsWith('/books/new') || pathname.startsWith('/books/') && pathname.endsWith('/edit')) {
    if (!token) {
      return NextResponse.redirect(new URL('/login', request.url));
    }

    try {
      const decoded = jwtDecode<JWTPayload>(token);
      const now = Math.floor(Date.now() / 1000);
      if (decoded.exp < now) {
        const response = NextResponse.redirect(new URL('/login', request.url));
        response.cookies.delete(TOKEN_NAME);
        return response;
      }
    } catch {
      const response = NextResponse.redirect(new URL('/login', request.url));
      response.cookies.delete(TOKEN_NAME);
      return response;
    }
  }

  // Si está en login/register y ya tiene token, ir a /books
  if ((pathname === '/login' || pathname === '/register') && token) {
    return NextResponse.redirect(new URL('/books', request.url));
  }

  return NextResponse.next();
}

export const config = {
  matcher: ['/books/:path*', '/login', '/register'],
};
