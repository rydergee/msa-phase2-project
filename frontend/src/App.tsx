import { BrowserRouter as Router, Routes, Route, Navigate, Link } from 'react-router-dom';
import './App.css';
import { AuthProvider, useAuth, withAuth } from './contexts/AuthContext';

// Import components (we'll create these next)
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import Profile from './pages/Profile';

// Create protected versions of components
const ProtectedDashboard = withAuth(Dashboard);
const ProtectedProfile = withAuth(Profile);

// Navigation component that's aware of authentication state
function Navigation() {
  const { isAuthenticated, user, logout } = useAuth();

  const handleLogout = async () => {
    try {
      await logout();
    } catch (error) {
      console.error('Logout failed:', error);
    }
  };

  return (
    <nav className="bg-blue-600 text-white p-4">
      <div className="container mx-auto flex justify-between items-center">
        <Link to="/" className="text-xl font-bold hover:text-blue-200">
          MockMate
        </Link>
        <div className="space-x-4">
          <Link to="/" className="hover:text-blue-200">Home</Link>
          
          {isAuthenticated ? (
            <>
              <Link to="/dashboard" className="hover:text-blue-200">Dashboard</Link>
              <Link to="/profile" className="hover:text-blue-200">Profile</Link>
              <span className="text-blue-200">Welcome, {user?.firstName}!</span>
              <button
                onClick={handleLogout}
                className="hover:text-blue-200 bg-transparent border-none cursor-pointer"
              >
                Logout
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="hover:text-blue-200">Login</Link>
              <Link to="/register" className="hover:text-blue-200">Register</Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
}

// Simple layout component
function Layout({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen bg-gray-50">
      <Navigation />
      <main className="container mx-auto py-8 px-4">
        {children}
      </main>
    </div>
  );
}

function App() {
  return (
    <AuthProvider>
      <Router>
        <Layout>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/dashboard" element={<ProtectedDashboard />} />
            <Route path="/profile" element={<ProtectedProfile />} />
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
}

export default App
