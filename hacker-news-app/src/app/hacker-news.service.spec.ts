import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HackerNewsService } from './hacker-news.service';
import { HackerNewsItem } from './hacker-news-item';

describe('HackerNewsService', () => {
  let service: HackerNewsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [HackerNewsService]
    });
    service = TestBed.inject(HackerNewsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch new stories', (done: DoneFn) => {
    const mockStories: HackerNewsItem[] = [
      {
        id: 1,
        title: 'Test Story 1',
        url: 'http://example.com/1',
        by: 'author1',
        time: 1234567890,
        score: 100,
        descendants: 50,
        type: 'story'
      },
      {
        id: 2,
        title: 'Test Story 2',
        url: 'http://example.com/2',
        by: 'author2',
        time: 1234567891,
        score: 200,
        descendants: 75,
        type: 'story'
      }
    ];

    service.getNewStories(1, 20).subscribe((stories) => {
      expect(stories.length).toBe(2);
      expect(stories).toEqual(mockStories);
      done();
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/newstories?page=1&pageSize=20`);
    expect(req.request.method).toBe('GET');
    req.flush(mockStories);
  });

  it('should search stories', (done: DoneFn) => {
    const mockStories: HackerNewsItem[] = [
      {
        id: 1,
        title: 'C# Programming',
        url: 'http://example.com/1',
        by: 'author1',
        time: 1234567890,
        score: 100,
        descendants: 50,
        type: 'story'
      }
    ];

    service.searchStories('C#', 1, 20).subscribe((stories) => {
      expect(stories.length).toBe(1);
      expect(stories[0].title).toContain('C#');
      done();
    });

    const req = httpMock.expectOne(`${service['apiUrl']}/search?query=C%23&page=1&pageSize=20`);
    expect(req.request.method).toBe('GET');
    req.flush(mockStories);
  });
});