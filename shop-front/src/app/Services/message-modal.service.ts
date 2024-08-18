import { EventEmitter, Injectable } from '@angular/core';
import { ModalNavigateService } from './modal-navigate.service';
import { MessageComponent } from '../components/modal/message/message.component';
import { Message, MessageType } from '../interfaces/message';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class MessageModalService {
  constructor(private modalService: ModalNavigateService) {}

  messageEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  async showSuccessMessage(Message: string, Title: string): Promise<boolean> {
    const messageData: Message = {
      Message,
      Title,
      Type: MessageType.Success,
    };
    this.modalService.goToModal(MessageComponent.Path, messageData);
    return await firstValueFrom(this.messageEvent);
  }
}
