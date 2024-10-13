# Security Learnings

## Incidents

[Midnight blizzard](https://msrc.microsoft.com/blog/2024/01/microsoft-actions-following-attack-by-nation-state-actor-midnight-blizzard/)

- password spray attack

[Crowdstrike](https://www.crowdstrike.com/falcon-content-update-remediation-and-guidance-hub/)

- Ops also need to be tested: dev testing, rollback testing

[Storm-0558](https://www.microsoft.com/en-us/security/blog/2023/07/14/analysis-of-storm-0558-techniques-for-unauthorized-email-access/?msockid=10236472bc406e1d106d77babd876f9f)

- long lived token signing key
- out-of-dated token validation logic
- non-standard AuthN pattern
- not able to revoke centrally
- excessive app permission
- Secrets across boundaries
- 
