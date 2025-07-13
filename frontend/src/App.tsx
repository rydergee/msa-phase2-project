import { BrowserRouter as Router, Routes, Route, Navigate, Link } from 'react-router-dom';
import './App.css';
import { AuthProvider, useAuth, withAuth } from './contexts/AuthContext';

// Import components (we'll create these next)
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Dashboard from './pages/Dashboard';
import Profile from './pages/Profile';

// Import Journal components
import JournalList from './components/Journal/JournalList';
import JournalForm from './components/Journal/JournalForm';
import JournalDetail from './components/Journal/JournalDetail';

// Create protected versions of components
const ProtectedDashboard = withAuth(Dashboard);
const ProtectedProfile = withAuth(Profile);
const ProtectedJournalList = withAuth(JournalList);
const ProtectedJournalForm = withAuth(JournalForm);
const ProtectedJournalDetail = withAuth(JournalDetail);

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
    <nav className="bg-gradient-to-r from-orange-200 to-orange-300 shadow-lg border-b border-orange-200">
      <div className="container mx-auto px-6 py-4">
        <div className="flex justify-between items-center">
          <Link to="/" className="text-2xl font-bold text-orange-800 hover:text-orange-900 transition-colors duration-200">
            MockMate
          </Link>
          <div className="flex items-center space-x-6">
            <Link to="/" className="text-orange-700 hover:text-orange-900 font-medium transition-colors duration-200">
              Home
            </Link>
            
            {isAuthenticated ? (
              <>
                <Link to="/dashboard" className="text-orange-700 hover:text-orange-900 font-medium transition-colors duration-200">
                  Dashboard
                </Link>
                <Link to="/journal" className="text-orange-700 hover:text-orange-900 font-medium transition-colors duration-200">
                  Journal
                </Link>
                <Link to="/profile" className="text-orange-700 hover:text-orange-900 font-medium transition-colors duration-200">
                  Profile
                </Link>
                <div className="flex items-center space-x-4">
                  <span className="text-orange-800 font-medium">Welcome, {user?.firstName}!</span>
                  <button
                    onClick={handleLogout}
                    className="bg-orange-100 hover:bg-orange-200 text-orange-800 font-medium py-2 px-4 rounded-lg transition-colors duration-200 border border-orange-300"
                  >
                    Logout
                  </button>
                </div>
              </>
            ) : (
              <>
                <Link to="/login" className="text-orange-700 hover:text-orange-900 font-medium transition-colors duration-200">
                  Login
                </Link>
                <Link 
                  to="/register" 
                  className="bg-orange-400 hover:bg-orange-500 text-white font-medium py-2 px-4 rounded-lg transition-colors duration-200 shadow-md"
                >
                  Register
                </Link>
              </>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}

// Simple layout component
function Layout({ children }: { children: React.ReactNode }) {
  return (
    <div className="min-h-screen bg-gradient-to-br from-orange-50 to-white">
      <Navigation />
      <main className="container mx-auto py-8 px-6">
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
            
            {/* Journal Routes */}
            <Route path="/journal" element={<ProtectedJournalList />} />
            <Route path="/journal/new" element={<ProtectedJournalForm />} />
            <Route path="/journal/edit/:id" element={<ProtectedJournalForm />} />
            <Route path="/journal/:id" element={<ProtectedJournalDetail />} />
            
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </Layout>
      </Router>
    </AuthProvider>
  );
}

export default App
