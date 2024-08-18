import { Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';

import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { Message, MessageType } from '../../../interfaces/message';
import { LoadingComponent } from '../../loading/loading.component';
import { MessageModalService } from '../../../Services/message-modal.service';

@Component({
  selector: 'app-message',
  standalone: true,
  imports: [LoadingComponent],
  templateUrl: './message.component.html',
  styleUrl: './message.component.css',
})
export class MessageComponent implements OnInit, OnDestroy {
  static Path = 'message';

  type!: MessageType;
  title!: string;
  message!: string;
  isLoaded = false;
  isDialog!: boolean;

  cardClasses = {
    [MessageType.Success.toString()]: 'bg-success-subtle',
  };

  buttonClasses = {
    [MessageType.Success.toString()]: 'btn-success',
  };

  constructor(
    private currentLocation: Location,
    private modal: ModalNavigateService,
    private messageModal: MessageModalService
  ) {}
  ngOnDestroy(): void {
    this.messageModal.messageEvent.emit(false);
  }

  ngOnInit() {
    if (!this.currentLocation.getState()) {
      this.onDismiss();
      return;
    }
    const {
      data: { Message, Type, Title },
    } = this.currentLocation.getState() as {
      data: Message;
    };

    this.type = Type;
    this.title = Title;
    this.message = Message;
    this.isLoaded = true;
    this.isDialog = this.type === MessageType.Dialog;
  }

  onAccept() {
    this.messageModal.messageEvent.emit(true);
    this.modal.dismiss();
  }

  onDismiss() {
    this.messageModal.messageEvent.emit(false);
    this.modal.dismiss();
  }
}
