import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { journalApi } from '../../services/journalApi';
import type { JournalEntry, JournalFilters } from '../../types/journal';
import JournalEntryCard from './JournalEntryCard';
import JournalFiltersComponent from './JournalFilters';
import LoadingSpinner from '../common/LoadingSpinner';

const JournalList: React.FC = () => {
  const [entries, setEntries] = useState<JournalEntry[]>([]);
  const [filteredEntries, setFilteredEntries] = useState<JournalEntry[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [filters, setFilters] = useState<JournalFilters>({});

  useEffect(() => {
    loadEntries();
  }, []);

  useEffect(() => {
    applyFilters();
  }, [entries, filters]);

  const loadEntries = async () => {
    try {
      setLoading(true);
      const data = await journalApi.getAll();
      setEntries(data);
      setError(null);
    } catch (err) {
      setError('Failed to load journal entries. Please try again.');
      console.error('Error loading journal entries:', err);
    } finally {
      setLoading(false);
    }
  };

  const applyFilters = () => {
    let filtered = [...entries];

    // Apply category filter
    if (filters.category) {
      filtered = filtered.filter(entry => 
        entry.category.toLowerCase().includes(filters.category!.toLowerCase())
      );
    }    // Apply search filter
    if (filters.search) {
      const searchTerm = filters.search.toLowerCase();
      filtered = filtered.filter(entry =>
        entry.question.toLowerCase().includes(searchTerm) ||
        entry.situation.toLowerCase().includes(searchTerm) ||
        entry.task.toLowerCase().includes(searchTerm) ||
        entry.action.toLowerCase().includes(searchTerm) ||
        entry.result.toLowerCase().includes(searchTerm)
      );
    }

    // Apply sorting
    if (filters.sortBy) {
      filtered.sort((a, b) => {
        let aValue, bValue;

        switch (filters.sortBy) {
          case 'createdAt':
            aValue = new Date(a.createdAt).getTime();
            bValue = new Date(b.createdAt).getTime();
            break;
          case 'updatedAt':
            aValue = new Date(a.updatedAt).getTime();
            bValue = new Date(b.updatedAt).getTime();
            break;
          case 'timesReviewed':
            aValue = a.timesReviewed;
            bValue = b.timesReviewed;
            break;
          default:
            return 0;
        }

        if (filters.sortOrder === 'desc') {
          return bValue - aValue;
        }
        return aValue - bValue;
      });
    }

    setFilteredEntries(filtered);
  };

  const handleDeleteEntry = async (id: number) => {
    if (!window.confirm('Are you sure you want to delete this journal entry?')) {
      return;
    }

    try {
      await journalApi.delete(id);
      setEntries(entries.filter(entry => entry.id !== id));
    } catch (err) {
      setError('Failed to delete journal entry. Please try again.');
      console.error('Error deleting journal entry:', err);
    }
  };

  if (loading) {
    return <LoadingSpinner />;
  }

  return (
    <div className="container mx-auto px-4 py-8">
      {/* Header */}
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold text-gray-900">My Journal</h1>
          <p className="text-gray-600 mt-2">
            Track your behavioral interview experiences using the STAR method
          </p>
        </div>
        <Link
          to="/journal/new"
          className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg font-medium transition-colors duration-200 flex items-center gap-2"
        >
          <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
          </svg>
          New Entry
        </Link>
      </div>

      {/* Filters */}
      <JournalFiltersComponent 
        filters={filters} 
        onFiltersChange={setFilters}
        entryCount={filteredEntries.length}
        totalCount={entries.length}
      />

      {/* Error Message */}
      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-lg mb-6">
          {error}
        </div>
      )}

      {/* Journal Entries */}
      {filteredEntries.length === 0 ? (
        <div className="text-center py-12">
          <div className="w-24 h-24 mx-auto mb-4 text-gray-300">
            <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.746 0 3.332.477 4.5 1.253v13C19.832 18.477 18.246 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" />
            </svg>
          </div>
          <h3 className="text-xl font-medium text-gray-900 mb-2">
            {entries.length === 0 ? 'No journal entries yet' : 'No entries match your filters'}
          </h3>
          <p className="text-gray-500 mb-6">
            {entries.length === 0 
              ? 'Start building your behavioral interview portfolio by creating your first journal entry.'
              : 'Try adjusting your search criteria or filters to see more entries.'
            }
          </p>
          {entries.length === 0 && (
            <Link
              to="/journal/new"
              className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg font-medium transition-colors duration-200 inline-flex items-center gap-2"
            >
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" />
              </svg>
              Create Your First Entry
            </Link>
          )}
        </div>
      ) : (
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          {filteredEntries.map((entry) => (
            <JournalEntryCard
              key={entry.id}
              entry={entry}
              onDelete={handleDeleteEntry}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default JournalList;
