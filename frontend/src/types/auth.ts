// Authentication types matching the backend
export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  university?: string;
  studyField?: string;
  createdAt: string;
  isActive: boolean;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
  university?: string;
  studyField?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  expires: string;
  user: User;
}

export interface UpdateProfileRequest {
  firstName: string;
  lastName: string;
  university?: string;
  studyField?: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

// API Response types
export interface ApiError {
  message: string;
  errors?: Record<string, string[]>;
}

export interface ApiResponse<T = any> {
  data?: T;
  error?: ApiError;
  success: boolean;
}
