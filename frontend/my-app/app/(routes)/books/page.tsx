import BookList from '@/features/books/components/BookList';
import Link from 'next/link';

export default function BooksPage() {
  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Libros</h1>
        <Link
          href="/books/new"
          className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
        >
          + Nuevo Libro
        </Link>
      </div>

      <BookList />
    </div>
  );
}