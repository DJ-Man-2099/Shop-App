export interface Message {
  Message: string;
  Title: string;
  Type: MessageType;
}

export enum MessageType {
  Success,
  Warning,
  Error,
  Dialog,
}
