// Journal Entry Types
export interface JournalEntry {
  id: number;
  question: string;
  situation: string;
  task: string;
  action: string;
  result: string;
  category: string;
  timesReviewed: number;
  lastReviewed?: string;
  createdAt: string;
  updatedAt: string;
}

export interface JournalEntrySummary {
  id: number;
  question: string;
  category: string;
  createdAt: string;
  timesReviewed: number;
}

export interface CreateJournalEntry {
  question: string;
  situation: string;
  task: string;
  action: string;
  result: string;
  category: string;
}

export interface UpdateJournalEntry {
  question: string;
  situation: string;
  task: string;
  action: string;
  result: string;
  category: string;
}

export interface JournalFormData {
  question: string;
  situation: string;
  task: string;
  action: string;
  result: string;
  category: string;
}

export interface JournalStats {
  totalEntries: number;
  categoryBreakdown: Record<string, number>;
  recentEntries: JournalEntrySummary[];
}

// Form Types
export interface JournalFormData {
  question: string;
  situation: string;
  task: string;
  action: string;
  result: string;
  category: string;
}

// Filter and Search Types
export interface JournalFilters {
  category?: string;
  search?: string;
  sortBy?: 'createdAt' | 'updatedAt' | 'timesReviewed';
  sortOrder?: 'asc' | 'desc';
}
