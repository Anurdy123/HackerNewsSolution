import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule } from '@angular/forms';
import { StoryListComponent } from './story-list';
import { HackerNewsService } from '../hacker-news.service';
import { of } from 'rxjs';
import { HackerNewsItem } from '../hacker-news-item';

describe('StoryListComponent', () => {
  let component: StoryListComponent;
  let fixture: ComponentFixture<StoryListComponent>;
  let hackerNewsService: HackerNewsService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, FormsModule, StoryListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(StoryListComponent);
    component = fixture.componentInstance;
    hackerNewsService = TestBed.inject(HackerNewsService);
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load stories on init', () => {
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
      }
    ];

    spyOn(hackerNewsService, 'getNewStories').and.returnValue(of(mockStories));
    
    component.ngOnInit();
    
    expect(component.stories.length).toBe(1);
    expect(component.stories[0].title).toBe('Test Story 1');
  });

  it('should search stories when search is called', () => {
    component.searchQuery = 'C#';
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

    spyOn(hackerNewsService, 'searchStories').and.returnValue(of(mockStories));
    
    component.onSearch();
    
    expect(component.stories.length).toBe(1);
    expect(component.stories[0].title).toContain('C#');
  });

  it('should change page when onPageChange is called', () => {
    const mockStories: HackerNewsItem[] = [];
    spyOn(hackerNewsService, 'getNewStories').and.returnValue(of(mockStories));
    
    component.onPageChange(2);
    
    expect(component.currentPage).toBe(2);
    expect(hackerNewsService.getNewStories).toHaveBeenCalledWith(2, 20);
  });
});