import { createContext, useContext, useState, useEffect } from 'react';
import type { ReactNode } from 'react';
import type { User, LoginRequest, RegisterRequest, UpdateProfileRequest, ChangePasswordRequest } from '../types/auth';
import { authService } from '../services/api';

interface AuthContextType {
  user: User | null;
  loading: boolean;
  login: (credentials: LoginRequest) => Promise<void>;
  register: (data: RegisterRequest) => Promise<void>;
  logout: () => Promise<void>;
  updateProfile: (data: UpdateProfileRequest) => Promise<void>;
  changePassword: (data: ChangePasswordRequest) => Promise<void>;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  // Check if user is authenticated on app load
  useEffect(() => {
    checkAuth();
  }, []);

  const checkAuth = async () => {
    try {
      setLoading(true);
      const token = localStorage.getItem('authToken');
      
      if (!token) {
        setLoading(false);
        return;
      }

      // Validate token and get user profile
      const isValid = await authService.validateToken();
      if (isValid) {
        const userData = await authService.getProfile();
        setUser(userData);
      }
    } catch (error) {
      console.error('Authentication check failed:', error);
      // Clear invalid token
      localStorage.removeItem('authToken');
      setUser(null);
    } finally {
      setLoading(false);
    }
  };

  const login = async (credentials: LoginRequest) => {
    try {
      const response = await authService.login(credentials);
      
      // Store token
      localStorage.setItem('authToken', response.token);
      
      // Set user data
      setUser(response.user);
    } catch (error) {
      console.error('Login failed:', error);
      throw error; // Re-throw for component handling
    }
  };

  const register = async (data: RegisterRequest) => {
    try {
      const response = await authService.register(data);
      
      // Store token
      localStorage.setItem('authToken', response.token);
      
      // Set user data
      setUser(response.user);
    } catch (error) {
      console.error('Registration failed:', error);
      throw error; // Re-throw for component handling
    }
  };

  const logout = async () => {
    try {
      // Call logout endpoint if token exists
      const token = localStorage.getItem('authToken');
      if (token) {
        await authService.logout();
      }
    } catch (error) {
      console.error('Logout request failed:', error);
      // Continue with local logout even if server request fails
    } finally {
      // Clear local storage and state
      localStorage.removeItem('authToken');
      setUser(null);
    }
  };

  const updateProfile = async (data: UpdateProfileRequest) => {
    try {
      const updatedUser = await authService.updateProfile(data);
      setUser(updatedUser);
    } catch (error) {
      console.error('Profile update failed:', error);
      throw error;
    }
  };

  const changePassword = async (data: ChangePasswordRequest) => {
    try {
      await authService.changePassword(data);
    } catch (error) {
      console.error('Password change failed:', error);
      throw error;
    }
  };

  const value: AuthContextType = {
    user,
    loading,
    login,
    register,
    logout,
    updateProfile,
    changePassword,
    isAuthenticated: !!user,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
}

// Custom hook to use the auth context
export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}

// Higher-order component for protected routes
export function withAuth<P extends object>(Component: React.ComponentType<P>) {
  return function AuthenticatedComponent(props: P) {
    const { isAuthenticated, loading } = useAuth();

    if (loading) {
      return (
        <div className="flex items-center justify-center min-h-screen">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
        </div>
      );
    }

    if (!isAuthenticated) {
      return (
        <div className="flex items-center justify-center min-h-screen">
          <div className="text-center">
            <h2 className="text-2xl font-bold text-gray-900 mb-4">Access Denied</h2>
            <p className="text-gray-600 mb-4">You need to be logged in to access this page.</p>
            <a href="/login" className="text-blue-600 hover:text-blue-800">
              Go to Login
            </a>
          </div>
        </div>
      );
    }

    return <Component {...props} />;
  };
}
