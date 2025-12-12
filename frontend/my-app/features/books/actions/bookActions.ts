/* eslint-disable @typescript-eslint/no-explicit-any */
'use server';

import { CreateBookDTO, UpdateBookDTO, BookResponse } from '../../types/book';
import { revalidatePath } from 'next/cache';
import { env } from '../../../config/env';
import { getAuthToken } from '../../../lib/auth';
const token = await getAuthToken();
/**
 * Crear un nuevo libro
 */
export const createBookAction = async (
  bookData: CreateBookDTO
): Promise<{ success: boolean; message: string; data?:  any }> => {
  try {
    
    if (!token) {
      return {
        success: false,
        message: 'No autenticado. Por favor inicia sesión.',
      };
    }
    const response = await fetch(`${env.NEXT_PUBLIC_BACKEND_API_URL}/api/book`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      },
      body: JSON.stringify(bookData),
    });
    
    const text = await response.text();


    if (!response.ok) {
      throw new Error(`Error ${response.status}: ${text || 'Sin respuesta del servidor'}`);
    }

    if (!text) {
      throw new Error('Respuesta vacía del servidor');
    }

    const data: BookResponse = JSON.parse(text);
    revalidatePath('/books');

    return {
      success: true,
      message: 'Libro creado exitosamente',
      data: data
    };
  } catch (error) {
    console.error('Error creating book:', error);
    return {
      success: false,
      message: error instanceof Error ? error.message : 'Error desconocido',
    };
  }
};

/**
 * Actualizar un libro
 */
export const updateBookAction = async (
  id: string,
  bookData: UpdateBookDTO
): Promise<{ success: boolean; message: string; data?: any }> => {
  try {
    const response = await fetch(`${env.NEXT_PUBLIC_BACKEND_API_URL}/api/book/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(bookData),
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || `Error: ${response.status}`);
    }

    const data: BookResponse = await response.json();
    revalidatePath('/books');
    revalidatePath(`/books/${id}`);

    return {
      success: true,
      message: 'Libro actualizado exitosamente',
      data: data.data,
    };
  } catch (error) {
    console.error(`Error updating book ${id}:`, error);
    return {
      success: false,
      message:  error instanceof Error ? error.message :  'Error desconocido',
    };
  }
};

/**
 * Eliminar un libro
 */
export const deleteBookAction = async (
  id: string
): Promise<{ success: boolean; message: string }> => {
  try {
    const response = await fetch(`${env.NEXT_PUBLIC_BACKEND_API_URL}/api/book/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    if (!response.ok) {
      const errorData = await response.json();
      throw new Error(errorData.message || `Error: ${response.status}`);
    }

    revalidatePath('/books');

    return {
      success: true,
      message: 'Libro eliminado exitosamente',
    };
  } catch (error) {
    console.error(`Error deleting book ${id}:`, error);
    return {
      success: false,
      message:  error instanceof Error ? error.message :  'Error desconocido',
    };
  }
};