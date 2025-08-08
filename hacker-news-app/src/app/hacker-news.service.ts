import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HackerNewsItem } from './hacker-news-item';

@Injectable({
  providedIn: 'root'
})
export class HackerNewsService {
  private apiUrl = 'http://localhost:5101/api/hackernews';

  constructor(private http: HttpClient) { }

  getNewStories(page: number = 1, pageSize: number = 20): Observable<HackerNewsItem[]> {
    const params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    return this.http.get<HackerNewsItem[]>(`${this.apiUrl}/newstories`, { params });
  }

  searchStories(query: string, page: number = 1, pageSize: number = 20): Observable<HackerNewsItem[]> {
    const params = new HttpParams()
      .set('query', query)
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());
    
    return this.http.get<HackerNewsItem[]>(`${this.apiUrl}/search`, { params });
  }
}