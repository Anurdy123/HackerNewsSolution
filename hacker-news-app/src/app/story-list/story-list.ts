import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HackerNewsService } from '../hacker-news.service';
import { HackerNewsItem } from '../hacker-news-item';

@Component({
  selector: 'app-story-list',
  templateUrl: './story-list.html',
  styleUrls: ['./story-list.css'],
  imports: [CommonModule, FormsModule]
})
export class StoryListComponent implements OnInit {
  stories: HackerNewsItem[] = [];
  currentPage: number = 1;
  pageSize: number = 20;
  totalPages: number = 1;
  isLoading: boolean = false;
  searchQuery: string = '';

  constructor(private hackerNewsService: HackerNewsService) { }

  ngOnInit(): void {
    this.loadStories();
  }

  loadStories(): void {
    this.isLoading = true;
    if (this.searchQuery) {
      this.hackerNewsService.searchStories(this.searchQuery, this.currentPage, this.pageSize)
        .subscribe({
          next: (stories) => {
            this.stories = stories;
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error loading stories:', error);
            this.isLoading = false;
          }
        });
    } else {
      this.hackerNewsService.getNewStories(this.currentPage, this.pageSize)
        .subscribe({
          next: (stories) => {
            this.stories = stories;
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error loading stories:', error);
            this.isLoading = false;
          }
        });
    }
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadStories();
  }

  onPageChange(page: number): void {
    this.currentPage = page;
    this.loadStories();
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.loadStories();
  }
}
