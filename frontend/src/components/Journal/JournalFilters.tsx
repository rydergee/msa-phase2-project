import React from 'react';
import type { JournalFilters } from '../../types/journal';

interface JournalFiltersProps {
  filters: JournalFilters;
  onFiltersChange: (filters: JournalFilters) => void;
  entryCount: number;
  totalCount: number;
}

const JournalFiltersComponent: React.FC<JournalFiltersProps> = ({
  filters,
  onFiltersChange,
  entryCount,
  totalCount
}) => {
  const categories = [
    'Software Development',
    'Leadership',
    'Communication',
    'Problem Solving',
    'Teamwork',
    'Database Development',
    'Project Management'
  ];

  const handleFilterChange = (key: keyof JournalFilters, value: string) => {
    onFiltersChange({
      ...filters,
      [key]: value || undefined
    });
  };

  const clearFilters = () => {
    onFiltersChange({});
  };

  const hasActiveFilters = filters.category || filters.search || filters.sortBy;

  return (
    <div className="bg-white rounded-xl shadow-lg border border-orange-100 p-6 mb-8">
      {/* Filter Controls */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4 mb-4">
        {/* Search */}
        <div>
          <label htmlFor="search" className="block text-sm font-semibold text-orange-700 mb-2">
            Search
          </label>
          <div className="relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <svg className="h-4 w-4 text-orange-400" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
            </div>
            <input
              type="text"
              id="search"
              placeholder="Search entries..."
              value={filters.search || ''}
              onChange={(e) => handleFilterChange('search', e.target.value)}
              className="block w-full pl-10 pr-3 py-2 border-2 border-orange-200 rounded-lg leading-5 bg-white placeholder-orange-400 focus:outline-none focus:placeholder-orange-300 focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200"
            />
          </div>
        </div>

        {/* Category Filter */}
        <div>
          <label htmlFor="category" className="block text-sm font-semibold text-orange-700 mb-2">
            Category
          </label>
          <select
            id="category"
            value={filters.category || ''}
            onChange={(e) => handleFilterChange('category', e.target.value)}
            className="block w-full px-3 py-2 border-2 border-orange-200 rounded-lg leading-5 bg-white focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200"
          >
            <option value="">All Categories</option>
            {categories.map((category) => (
              <option key={category} value={category}>
                {category}
              </option>
            ))}
          </select>
        </div>

        {/* Sort By */}
        <div>
          <label htmlFor="sortBy" className="block text-sm font-semibold text-orange-700 mb-2">
            Sort By
          </label>
          <select
            id="sortBy"
            value={filters.sortBy || ''}
            onChange={(e) => handleFilterChange('sortBy', e.target.value)}
            className="block w-full px-3 py-2 border-2 border-orange-200 rounded-lg leading-5 bg-white focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200"
          >
            <option value="">Default</option>
            <option value="createdAt">Date Created</option>
            <option value="updatedAt">Date Modified</option>
            <option value="timesReviewed">Times Reviewed</option>
          </select>
        </div>

        {/* Sort Order */}
        <div>
          <label htmlFor="sortOrder" className="block text-sm font-semibold text-orange-700 mb-2">
            Order
          </label>
          <select
            id="sortOrder"
            value={filters.sortOrder || 'desc'}
            onChange={(e) => handleFilterChange('sortOrder', e.target.value)}
            disabled={!filters.sortBy}
            className="block w-full px-3 py-2 border-2 border-orange-200 rounded-lg leading-5 bg-white focus:outline-none focus:ring-2 focus:ring-orange-400 focus:border-orange-400 transition-colors duration-200 disabled:bg-orange-50 disabled:text-orange-400"
          >
            <option value="desc">Newest First</option>
            <option value="asc">Oldest First</option>
          </select>
        </div>
      </div>

      {/* Filter Status and Clear */}
      <div className="flex items-center justify-between">
        <div className="text-sm text-orange-600">
          Showing {entryCount} of {totalCount} entries
          {hasActiveFilters && (
            <span className="ml-2 text-orange-700 font-semibold">
              ({entryCount === totalCount ? 'no filters applied' : 'filtered'})
            </span>
          )}
        </div>

        {hasActiveFilters && (
          <button
            onClick={clearFilters}
            className="text-sm text-orange-500 hover:text-orange-700 underline decoration-orange-300 hover:decoration-orange-500 transition-colors font-semibold"
          >
            Clear all filters
          </button>
        )}
      </div>
    </div>
  );
};

export default JournalFiltersComponent;
