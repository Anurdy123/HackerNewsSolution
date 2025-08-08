import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StoryListComponent } from './story-list/story-list';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, StoryListComponent],
  template: `
    <app-story-list></app-story-list>
  `,
  styles: [],
})
export class App {
  protected readonly title = signal('hacker-news-app');
}
