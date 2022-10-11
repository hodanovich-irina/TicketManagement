export const variables = {
    EventApiUrl : "https://localhost:44300/Event/",
    EventAreaApiUrl :"https://localhost:44300/EventArea/",
    EventAreaUIUrl : "http://localhost:3000/EventArea/",
    EventUIUrl : "http://localhost:3000/Event",
    EventSeatApiUrl : "https://localhost:44300/EventSeat/",
    EventSeatUIUrl : "http://localhost:3000/EventSeat/",
    PresentationUrl: "https://localhost:44380/",
    UserApiUrl : "https://localhost:44335/",
    TicketApiUrl : "https://localhost:44317/",
    TicketUIUrl : "http://localhost:3000/PurchaseHistory/",
    VenueApiUrl:   "https://localhost:44328/",
}

export const eventMethods = {
    GetAll : "GetAll",
    Add : "Add",
    Update : "Update",
    Delete : "Delete?id=",
    GetById : "GetById?id=",
    GetByParentId : "GetByParenId?id=",
    SplitSumbol : "/",
}

export const presentationMethods = {
    Login: "login",
    User: "User",
    Role: "Role",
    Ticket: "Ticket",
    Venue:"Venue",
    Register: "Account/Register",
    ThirdPartyImport: "ThirdPartyImport",
    BuyTicket: "ticket/Create/",
    DelteTicket: "ticket/Delete/"
}

export const userMethods = {
    Validate: "Account/validate?token=",
    GetCurrentUser : "Account/GetCurrentUser/",
    Update : "Account/Update/",
    UpdateBalance : "Account/UpdateBalance/",
    ChangePassword: "Account/ChangePassword/",
    GetCurrentUserRoles:"Role/GetCurrentUserRoles",
}

export const ticketMethods = {
    GetByParentStringId: "Ticket/GetByParentStringId?id=",
    GetUserTicketInfo: "Ticket/GetUserTicketInfo?id="
}

export const venueMethods = {
    GetAll: "layout/GetAll",
}

export const roleConstants = {
    User: "user",
    VenueManager: "venueManager",
    EventManager: "eventManager",
    Admin: "admin",
}