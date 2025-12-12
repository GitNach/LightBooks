import { updateBookAction } from "@/features/books/actions/bookActions";
import { UpdateBookDTO } from "@/features/types/book";
import { useRouter } from "next/router";
import { useState } from "react";

const updateBookForm: UpdateBookDTO = {
   title: "",
  isbn: "",
  description: "",
  purchasePrice: 0,
  rentalPricePerDay: 0,
  stockPurchase: 0,
  stockRental: 0,
  imageUrl: "",
  publishedDate: "",
  authorId: "",
  categoryId: "",
};
export default function EditBookPage() {
const [formData, setFormData] = useState<UpdateBookDTO>(updateBookForm);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const bookId = router.query.id as string;
 const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    setError("");
    setLoading(true);

    try {
      const result = await updateBookAction(bookId,formData);

      if (result.success && result.data?.id) {
        setFormData(updateBookForm);
        router.push(`/books/${result.data.id}`);
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError(err instanceof Error ? err.message : "Error desconocido");
    } finally {
      setLoading(false);
    }
  };
   return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="bg-white p-8 rounded-lg shadow-md w-full max-w-md">
        <h1 className="text-3xl font-bold mb-6 text-center">
          Agregar nuevo libro
        </h1>
        {error && (
          <div className="bg-red-100 border border-red-400 text-red-700 p-4 rounded mb-4">
            {error}
          </div>
        )}
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Título
            </label>
            <input
              type="text"
              value={formData.title}
              onChange={(e) =>
                setFormData({ ...formData, title: e.target.value })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="Título del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              ISBN
            </label>
            <input
              type="text"
              value={formData.isbn}
              onChange={(e) =>
                setFormData({ ...formData, isbn: e.target.value })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="ISBN del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Descripción
            </label>
            <input
              type="text"
              value={formData.description}
              onChange={(e) =>
                setFormData({ ...formData, description: e.target.value })
              }
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="Descripción del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Precio de compra
            </label>
            <input
              type="number"
              value={formData.purchasePrice}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  purchasePrice: Number(e.target.value),
                })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="Precio de compra del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Precio de alquiler por día
            </label>
            <input
              type="number"
              value={formData.rentalPricePerDay}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  rentalPricePerDay: Number(e.target.value),
                })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="Precio de alquiler por día del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Stock de compra
            </label>
            <input
              type="number"
              value={formData.stockPurchase}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  stockPurchase: Number(e.target.value),
                })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="Stock de compra del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Stock de alquiler
            </label>
            <input
              type="number"
              value={formData.stockRental}
              onChange={(e) =>
                setFormData({
                  ...formData,
                  stockRental: Number(e.target.value),
                })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="Stock de alquiler del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Imagen del libro (URL)
            </label>
            <input
              type="text"
              value={formData.imageUrl}
              onChange={(e) =>
                setFormData({ ...formData, imageUrl: e.target.value })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="URL de la imagen del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Fecha de publicación
            </label>
            <input
              type="date"
              value={formData.publishedDate}
              onChange={(e) =>
                setFormData({ ...formData, publishedDate: e.target.value })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="Fecha de publicación del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Autor ID
            </label>
            <input
              type="text"
              value={formData.authorId}
              onChange={(e) =>
                setFormData({ ...formData, authorId: e.target.value })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="ID del autor del libro"
            />
          </div>
          <div>
            <label className="block text-gray-700 text-sm font-semibold mb-2">
              Category ID
            </label>
            <input
              type="text"
              value={formData.categoryId}
              onChange={(e) =>
                setFormData({ ...formData, categoryId: e.target.value })
              }
              required
              className="w-full px-4 py-2 text-gray-700 border border-gray-300 rounded focus:outline-none focus:border-blue-500"
              placeholder="ID de la categoría del libro"
            />
          </div>
          <button
            type="submit"
            disabled={loading}
            className="w-full bg-blue-500 text-white font-semibold py-2 rounded hover:bg-blue-600 disabled:opacity-50 transition"
          >
            {loading ? "Creando Libro" : "Crear Libro"}
          </button>
        </form>
      </div>
    </div>
  );
}
