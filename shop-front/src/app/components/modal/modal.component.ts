import {
  Component,
  ElementRef,
  OnInit,
  Renderer2,
  ViewChild,
} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ModalNavigateService } from '../../Services/modal-navigate.service';

@Component({
  selector: 'app-modal',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css',
})
export class ModalComponent implements OnInit {
  @ViewChild('modal', { static: true }) modalElement!: ElementRef;

  constructor(
    private modal: ModalNavigateService,
    private renderer: Renderer2
  ) {}

  ngOnInit(): void {
    this.modal.toggle.subscribe((isOpen) => {
      if (isOpen) {
        this.renderer.removeClass(this.modalElement.nativeElement, 'hide');
      } else {
        this.renderer.addClass(this.modalElement.nativeElement, 'hide');
      }
    });
  }

  dismiss() {
    this.modal.dismiss();
  }
}
