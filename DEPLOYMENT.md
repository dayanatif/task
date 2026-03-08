# TSManager Docker Deployment on RHEL VM

---

## Deploy Without Domain (Simplest)

No Traefik, no domain. Access the app via **http://VM_IP:8080**

```bash
cd tsmanager
cp env.example .env
docker compose -f docker-compose.standalone.yml up -d --build
```

Then open **http://localhost:8080** (from the VM) or **http://YOUR_VM_IP:8080** (from another machine).

---

## Deploy With Traefik (Domain Required)

Uses your existing Traefik network and requires a domain (e.g. tsmanager.yourdomain.com).

### Quick Start

```bash
# 1. Verify Traefik network name
docker network ls

# 2. Update docker-compose.yml if your Traefik network has a different name

# 3. Create .env from env.example and set TRAEFIK_HOST
cp env.example .env && nano .env

# 4. Deploy
docker compose up -d --build
```

---

## Prerequisites

- RHEL 8/9 VM with Docker and Docker Compose installed
- Traefik already running with its Docker network
- Git (to clone the repo)

---

## Step 1: Find Your Traefik Network Name

On the VM, list Docker networks and note the Traefik network name:

```bash
docker network ls
```

Look for a network used by Traefik (e.g. `traefik`, `traefik-public`, `docker_traefik`). You will use this exact name in the compose file.

---

## Step 2: Update docker-compose.yml Network (if needed)

If your Traefik network is **not** named `traefik`, edit `docker-compose.yml` and change the network name under `networks`:

```yaml
networks:
  traefik:
    external: true
```

Replace `traefik` with your actual network name (e.g. `traefik-public`).

---

## Step 3: Clone the Repository

```bash
cd /opt   # or your preferred directory
sudo git clone <your-repo-url> tsmanager
cd tsmanager
```

---

## Step 4: Create Environment File

```bash
cp env.example .env
nano .env
```

Edit `.env` with your values. The new MySQL container uses **fixed credentials** (see `env.example`); you can use them as-is or change before first run.

| Variable | Description |
|----------|-------------|
| `TRAEFIK_HOST` | Domain for the app (e.g. `tsmanager.yourdomain.com`) |
| `MYSQL_DATABASE` | Database name (default `tsmanager_db`) |
| `MYSQL_USER` | MySQL app user (default `tsmanager_app`) |
| `MYSQL_PASSWORD` | MySQL app password (set in `.env`) |
| `MYSQL_ROOT_PASSWORD` | MySQL root password (set in `.env`) |
| `SMTP_*` | SMTP settings for email |

---

## Step 5: Run the Application

```bash
# Build and start
sudo docker compose up -d --build

# Check status
sudo docker compose ps
```

---

## Step 6: Migrations

Migrations run automatically when the app starts. No manual step is required.

---

## Step 7: Configure Traefik (if needed)

Ensure Traefik is configured to use the same network. In your Traefik config (e.g. `traefik.yml` or `traefik.toml`):

```yaml
providers:
  docker:
    network: traefik   # must match the network name in docker-compose
```

For HTTPS, add a router with the `websecure` entrypoint and your certificate resolver. Example labels you can add to `tsmanager` in `docker-compose.yml`:

```yaml
- "traefik.http.routers.tsmanager.entrypoints=web,websecure"
- "traefik.http.routers.tsmanager-https.rule=Host(\`tsmanager.yourdomain.com\`)"
- "traefik.http.routers.tsmanager-https.entrypoints=websecure"
- "traefik.http.routers.tsmanager-https.tls=true"
- "traefik.http.routers.tsmanager-https.tls.certresolver=letsencrypt"
```

---

## Step 8: Verify

- Open `http://TRAEFIK_HOST` (or `https://TRAEFIK_HOST` if HTTPS is configured)
- Default admin: `mohid.rehman04@gmail.com` (password set during first seed – check `SeedData` for details)

---

## Accessing MySQL from the VM

The MySQL container exposes port **3306** on the host. Use the same credentials as in your `.env`.

**App user (tsmanager_db):**
```bash
mysql -h 127.0.0.1 -P 3306 -u tsmanager_app -p tsmanager_db
# Password: (value of MYSQL_PASSWORD in .env, e.g. TsManagerApp2025!)
```

**Root user (all databases):**
```bash
mysql -h 127.0.0.1 -P 3306 -u root -p
# Password: (value of MYSQL_ROOT_PASSWORD in .env, e.g. TsManagerRoot2025!)
```

Install MySQL client on RHEL if needed:
```bash
sudo dnf install mysql
```

---

## Useful Commands

**Standalone (no domain):**
```bash
COMPOSE="-f docker-compose.standalone.yml"
docker compose $COMPOSE logs -f tsmanager
docker compose $COMPOSE down
docker compose $COMPOSE down -v   # also removes database
docker compose $COMPOSE up -d --build
```

**With Traefik:** Omit the `-f` flag and use `docker compose` as normal.

---

## Troubleshooting

### Container cannot connect to Traefik network

```bash
# Ensure the Traefik network exists
docker network ls | grep traefik

# Create it if missing (only if Traefik uses a different setup)
docker network create traefik
```

### 502 Bad Gateway

- Confirm the app container is running: `docker compose ps`
- Check logs: `docker compose logs tsmanager`
- Ensure the Traefik network name in `docker-compose.yml` matches the one Traefik uses

### Database connection failed

- Wait for MySQL to be healthy: `docker compose ps` (look for “healthy”)
- Verify `.env` credentials match the MySQL container
- Check connectivity: `docker compose exec mysql mysql -u tsmanager -p -e "SELECT 1"`

### Wrong network name

If you see “network traefik not found”, update the `traefik` network name in `docker-compose.yml` to match your existing Traefik network.
