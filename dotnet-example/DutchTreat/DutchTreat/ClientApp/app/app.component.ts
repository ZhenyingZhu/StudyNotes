import { Component } from '@angular/core';

@Component({
  selector: 'the-shop',
  template: `
    <div style="text-align:center">
      <h1>
        Welcome to {{title}}!
      </h1>
    </div>
    <router-outlet></router-outlet>
  `,
  styles: []
})
export class AppComponent {
  title = 'Dutch Treat';
}
