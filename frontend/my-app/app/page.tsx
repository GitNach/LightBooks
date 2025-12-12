

export default function Home() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <h1 className="text-5xl font-bold text-center sm:text-left">
          Bienvenido a <span className="text-blue-600">DevLights Biblioteca</span>
        </h1>
        <p className="mt-6 text-lg text-gray-600 dark:text-gray-300 text-center sm:text-left">
          Tu plataforma para gestionar y explorar una amplia colección de libros.
        </p>
        
      </main>
    </div>
  );
}
