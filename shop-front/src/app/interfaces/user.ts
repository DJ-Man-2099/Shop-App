export interface UserOutput {
  firstName: string;
  lastName: string;
  userName: string;
  id: number;
  token: string;
  roles: string[];
}

export interface UserSignup {
  firstName: string;
  lastName: string;
  userName: string;
  password: string;
}

export interface UserLogin {
  userName: string;
  password: string;
}

export interface InnerUser {
  name: string;
  role: string;
}
