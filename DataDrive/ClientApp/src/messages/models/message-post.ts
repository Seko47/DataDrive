export class MessagePost {

    public toUserUsername: string;
    public content: string;

    constructor(username: string, content: string) {

        this.toUserUsername = username;
        this.content = content;
    }
}
