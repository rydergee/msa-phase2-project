function Login() {
  return (
    <div className="max-w-md mx-auto bg-white rounded-lg shadow-md p-6">
      <h1 className="text-2xl font-bold text-center mb-6">Login</h1>
      <p className="text-center text-gray-600">
        Login form will be implemented in TASK-007
      </p>
      <div className="mt-6 text-center">
        <a href="/register" className="text-blue-600 hover:text-blue-800">
          Don't have an account? Register here
        </a>
      </div>
    </div>
  );
}

export default Login;
