/* eslint-disable @next/next/no-img-element */
// Server Component
import Link from 'next/link';
import { BookDTO } from '@/features/types/book';

interface BookCardProps {
  book:  BookDTO;
}

export default function BookCard({ book }: BookCardProps) {
  const hasPurchaseStock = book.stockPurchase > 0;
  const hasRentalStock = book.stockRental > 0;

  return (
    <div className="border rounded-lg overflow-hidden hover:shadow-lg transition bg-blue">
      {/* Imagen del libro */}
      {book.imageUrl && (
        <div className="w-full h-48 bg-gray-200 overflow-hidden">
          <img
            src={book.imageUrl}
            alt={book. title}
            className="w-full h-full object-cover hover:scale-105 transition"
          />
        </div>
      )}

      <div className="p-4 flex flex-col h-full">
        {/* Título y Autor */}
        <h3 className="text-lg font-bold truncate">{book.title}</h3>
        <p className="text-sm text-gray-600">{book.authorName}</p>
        <p className="text-xs text-gray-500">{book.categoryName}</p>

        {/* Precios */}
        <div className="mt-3 flex justify-between text-sm">
          <div>
            <p className="text-gray-600">Compra</p>
            <p className="font-bold text-lg">${book. purchasePrice}</p>
          </div>
          <div>
            <p className="text-gray-600">Renta/día</p>
            <p className="font-bold text-lg">${book.rentalPricePerDay}</p>
          </div>
        </div>

        {/* Stock */}
        <div className="mt-3 flex gap-2 text-xs">
          {hasPurchaseStock && (
            <span className="bg-green-100 text-green-800 px-2 py-1 rounded">
              Compra:  {book.stockPurchase}
            </span>
          )}
          {hasRentalStock && (
            <span className="bg-blue-100 text-blue-800 px-2 py-1 rounded">
              Renta: {book.stockRental}
            </span>
          )}
        </div>

        {/* Botones */}
        <div className="mt-4 flex gap-2">
          <Link
            href={`/books/${book.id}`}
            className="flex-1 bg-blue-500 text-white text-center py-2 rounded hover:bg-blue-600 transition text-sm"
          >
            Ver Detalle
          </Link>

          <Link
            href={`/books/${book.id}/edit`}
            className="flex-1 bg-yellow-500 text-white text-center py-2 rounded hover: bg-yellow-600 transition text-sm"
          >
            Editar
          </Link>
        </div>
      </div>
    </div>
  );
}