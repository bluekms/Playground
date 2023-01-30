namespace CommonLibrary.Models
{
    /// <summary>
    /// Administrator : 모든 Role의 유저를 생성 가능
    /// Developer : 개발자 개발서버의 모든 권한. 유저 롤은 변경 불가
    /// OpUser : 내부 서버나 운영자. 실서버의 거의 모든 권한. 유저 롤 변경 가능
    /// WhitelistUser : 점검 시간에 접속 가능한 것을 제외하고 유저랑 동일
    /// User : 일반 유저 (AuthServer에서 가입)
    /// BanUser : 접속 불가능한 유저
    /// </summary>
    public enum AccountRoles
    {
        Administrator,
        Developer,
        OpUser,
        WhitelistUser,
        User,
        BanUser,
    }

    /// <summary>
    /// Auth : N대. 일반 유저 회원가입. 계정 인증. 점검 공지. 내려가지 않음
    /// Operation : 1대. 운영 관련 API들의 처리
    /// World : M대. 실질적인 로직 처리의 주체
    /// IncrementValueService : Redis를 이용해 1씩 증가하는 번호를 발급하는 서비스
    /// </summary>
    public enum ServerRoles
    {
        Auth,
        Operation,
        World,
    }
}