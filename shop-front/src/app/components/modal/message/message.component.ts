import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { Message } from '../../../interfaces/message';
import { LoadingComponent } from '../../loading/loading.component';

@Component({
  selector: 'app-message',
  standalone: true,
  imports: [LoadingComponent],
  templateUrl: './message.component.html',
  styleUrl: './message.component.css',
})
export class MessageComponent implements OnInit {
  static Path = 'message';

  type?: string;
  title!: string;
  messageType = 'btn-primary';
  message!: Message;
  onAccept!: () => void;
  isLoaded = false;

  constructor(
    private currentLocation: Location,
    private modal: ModalNavigateService
  ) {}

  ngOnInit() {
    if (!this.currentLocation.getState()) {
      this.onDismiss();
      return;
    }
    const {
      data: { type, message, onAccept, title },
    } = this.currentLocation.getState() as {
      data: {
        type?: string;
        title: string;
        message: Message;
        onAccept: () => void;
      };
    };

    if (type) {
      this.type = type;
    }
    this.title = title;
    this.message = message;
    this.onAccept = onAccept;
    this.isLoaded = true;

    if (this.type === 'warning') {
      this.messageType = 'btn-danger';
    }
  }

  onDismiss() {
    this.modal.dismiss();
  }
}
