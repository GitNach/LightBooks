/* eslint-disable @next/next/no-img-element */
// app/books/[id]/page. tsx
import { getBookById } from "@/features/books/services/bookService";
import Link from "next/link";

interface BookDetailPageProps {
  params: {
    id: string;
  };
}

export default async function BookDetailPage({ params }: BookDetailPageProps) {
  const { id } = await params;
  const book = await getBookById(id);

  const hasPurchaseStock = book.stockPurchase > 0;
  const hasRentalStock = book.stockRental > 0;

  return (
    <div className="p-6 max-w-4xl mx-auto">
      {/* Volver */}
      <Link href="/books" className="text-blue-500 hover:underline mb-6 block">
        ← Volver a Libros
      </Link>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Imagen */}
        <div className="md:col-span-1">
          {book.imageUrl && (
            <img
              src={book.imageUrl}
              alt={book.title}
              className="w-full rounded-lg shadow-lg"
            />
          )}
        </div>

        {/* Información */}
        <div className="md:col-span-2">
          <h1 className="text-4xl font-bold mb-2">{book.title}</h1>
          <p className="text-xl text-gray-600 mb-4">
            {typeof book.author === 'string' ? book.author : book.author.fullName}
          </p>
          <p className="text-sm text-gray-500 mb-6">
            Categoría: {typeof book.category === 'string' ? book.category : book.category.name}
          </p>

          {/* Descripción */}
          <div className="mb-6">
            <h2 className="text-lg font-semibold mb-2">Descripción</h2>
            <p className="text-gray-700">
              {book.description || "Sin descripción disponible"}
            </p>
          </div>

          {/* Detalles */}
          <div className="grid grid-cols-2 gap-4 mb-6">
            <div className="bg-gray-50 p-4 rounded">
              <p className="text-gray-600 text-sm">ISBN</p>
              <p className="font-semibold text-black">{book.isbn}</p>
            </div>

            <div className="bg-gray-50 p-4 rounded">
              <p className="text-gray-600 text-sm">Precio Compra</p>
              <p className="font-semibold text-lg text-black">
                ${book.purchasePrice}
              </p>
            </div>

            <div className="bg-gray-50 p-4 rounded">
              <p className="text-gray-600 text-sm">Precio Renta/Día</p>
              <p className="font-semibold text-lg text-black">
                ${book.rentalPricePerDay}
              </p>
            </div>
          </div>

          {/* Stock */}
          <div className="grid grid-cols-2 gap-4 mb-6">
            <div
              className={`p-4 rounded ${
                hasPurchaseStock
                  ? "bg-green-50 border border-green-200"
                  : "bg-red-50 border border-red-200"
              }`}
            >
              <p className="text-sm font-semibold text-black">Stock Compra</p>
              <p
                className={`text-2xl font-bold ${
                  hasPurchaseStock ? "text-green-600" : "text-red-600"
                }`}
              >
                {book.stockPurchase}
              </p>
            </div>

            <div
              className={`p-4 rounded ${
                hasRentalStock
                  ? "bg-blue-50 border border-blue-200"
                  : "bg-red-50 border border-red-200"
              }`}
            >
              <p className="text-sm font-semibold text-black">Stock Renta</p>
              <p
                className={`text-2xl font-bold ${
                  hasRentalStock ? "text-blue-600" : "text-red-600"
                }`}
              >
                {book.stockRental}
              </p>
            </div>
          </div>

          {/* Botones */}
          <div className="flex gap-3">
            <Link
              href={`/books/${book.id}/edit`}
              className="flex-1 bg-yellow-500 text-white px-6 py-3 rounded hover:bg-yellow-600 transition font-semibold text-center"
            >
              Comprar Libro
            </Link>

            <Link
              href={`/books/${book.id}/delete`}
              className="flex-1 bg-green-500 text-white px-6 py-3 rounded hover:bg-red-600 transition font-semibold text-center"
            >
              Alquilar Libro
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}
