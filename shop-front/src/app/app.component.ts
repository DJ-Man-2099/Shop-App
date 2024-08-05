import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BasecategorypriceComponent } from './components/basecategoryprice/basecategoryprice.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, BasecategorypriceComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'shop-front';
}
