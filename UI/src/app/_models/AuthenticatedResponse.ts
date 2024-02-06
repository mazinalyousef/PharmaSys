export interface AuthenticatedResponse

  {

    id : string;
    username :string;
    expiration :Date;
    roles : string [];
    token: string;
  }