import apiClient from './api';
import type { 
  JournalEntry, 
  CreateJournalEntry, 
  UpdateJournalEntry, 
  JournalStats 
} from '../types/journal';

export const journalApi = {
  // Get all journal entries for the authenticated user
  getAll: async (): Promise<JournalEntry[]> => {
    const response = await apiClient.get('/journal');
    return response.data;
  },

  // Get a specific journal entry by ID
  getById: async (id: number): Promise<JournalEntry> => {
    const response = await apiClient.get(`/journal/${id}`);
    return response.data;
  },

  // Get journal entries by category
  getByCategory: async (category: string): Promise<JournalEntry[]> => {
    const response = await apiClient.get(`/journal/category/${encodeURIComponent(category)}`);
    return response.data;
  },

  // Get recent journal entries
  getRecent: async (count: number = 5): Promise<JournalEntry[]> => {
    const response = await apiClient.get(`/journal/recent?count=${count}`);
    return response.data;
  },

  // Search journal entries
  search: async (query: string): Promise<JournalEntry[]> => {
    const response = await apiClient.get(`/journal/search?query=${encodeURIComponent(query)}`);
    return response.data;
  },

  // Get most reviewed journal entries
  getMostReviewed: async (count: number = 5): Promise<JournalEntry[]> => {
    const response = await apiClient.get(`/journal/most-reviewed?count=${count}`);
    return response.data;
  },

  // Get journal statistics
  getStats: async (): Promise<JournalStats> => {
    const response = await apiClient.get('/journal/stats');
    return response.data;
  },

  // Create a new journal entry
  create: async (entry: CreateJournalEntry): Promise<JournalEntry> => {
    const response = await apiClient.post('/journal', entry);
    return response.data;
  },

  // Update an existing journal entry
  update: async (id: number, entry: UpdateJournalEntry): Promise<JournalEntry> => {
    const response = await apiClient.put(`/journal/${id}`, entry);
    return response.data;
  },

  // Delete a journal entry
  delete: async (id: number): Promise<void> => {
    await apiClient.delete(`/journal/${id}`);
  },
};
